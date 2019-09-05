﻿using IdentityServer.Helpers;
using IdentityServer.Interfaces.Processors;
using IdentityServer.Interfaces.Providers;
using IdentityServer.Repositories.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ExternalAuthenticationGrant
{
    public class ExternalAuthenticationGrant<TUser> : IExtensionGrantValidator where TUser : IdentityUser, new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IProviderRepository _providerRepository;
        private readonly IFacebookAuthProvider _facebookAuthProvider;
        private readonly IGoogleAuthProvider _googleAuthProvider;        
        private readonly INonEmailUserProcessor _nonEmailUserProcessor;
        private readonly IEmailUserProcessor _emailUserProcessor;
        public ExternalAuthenticationGrant(
            UserManager<TUser> userManager,
            IProviderRepository providerRepository,
            IFacebookAuthProvider facebookAuthProvider,
            IGoogleAuthProvider googleAuthProvider,            
            INonEmailUserProcessor nonEmailUserProcessor,
            IEmailUserProcessor emailUserProcessor
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
            _facebookAuthProvider = facebookAuthProvider ?? throw new ArgumentNullException(nameof(facebookAuthProvider));
            _googleAuthProvider = googleAuthProvider ?? throw new ArgumentNullException(nameof(googleAuthProvider));            
            _nonEmailUserProcessor = nonEmailUserProcessor ?? throw new ArgumentNullException(nameof(nonEmailUserProcessor));
            _emailUserProcessor = emailUserProcessor ?? throw new ArgumentNullException(nameof(nonEmailUserProcessor));

            _providers = new Dictionary<ProviderType, IExternalAuthProvider>
            {
                 {ProviderType.Facebook, _facebookAuthProvider},
                 {ProviderType.Google, _googleAuthProvider}
            };
        }


        private Dictionary<ProviderType, IExternalAuthProvider> _providers;

        public string GrantType => "external";



        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var provider = context.Request.Raw.Get("provider");
            if (string.IsNullOrWhiteSpace(provider))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }


            var token = context.Request.Raw.Get("external_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid external token");
                return;
            }

            var requestEmail = context.Request.Raw.Get("email");

            var providerType = (ProviderType)Enum.Parse(typeof(ProviderType), provider, true);

            if (!Enum.IsDefined(typeof(ProviderType), providerType))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }

            var userInfo = _providers[providerType].GetUserInfo(token);

            if (userInfo == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "couldn't retrieve user info from specified provider, please make sure that access token is not expired.");
                return;
            }

            var externalId = userInfo.Value<string>("id");
            if (!string.IsNullOrWhiteSpace(externalId))
            {

                var user = await _userManager.FindByLoginAsync(provider, externalId);
                if (null != user)
                {
                    user = await _userManager.FindByIdAsync(user.Id);
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    context.Result = new GrantValidationResult(user.Id, provider, userClaims, provider, null);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(requestEmail))
            {
                context.Result = await _nonEmailUserProcessor.ProcessAsync(userInfo, provider);
                return;
            }

            context.Result = await _emailUserProcessor.ProcessAsync(userInfo, requestEmail, provider);
            return;
        }
    }
}
