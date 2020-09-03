using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;
using System;
using UnityEngine;

namespace HmsPlugin
{
    public class AccountManager : MonoBehaviour
    {

        public static AccountManager GetInstance(string name = "AccountManager") => GameObject.Find(name).GetComponent<AccountManager>();

        private static HuaweiIdAuthService DefaultAuthService
        {
            get
            {
                Debug.Log("[HMS]: GET AUTH");
                var authParams = new HuaweiIdAuthParamsHelper(HuaweiIdAuthParams.DEFAULT_AUTH_REQUEST_PARAM).SetIdToken().CreateParams();
                Debug.Log("[HMS]: AUTHPARAMS AUTHSERVICE" + authParams);
                var result = HuaweiIdAuthManager.GetService(authParams);
                Debug.Log("[HMS]: RESULT AUTHSERVICE"+ result);
                return result;
            }
        }

        public AuthHuaweiId HuaweiId { get; private set; }
        public Action<AuthHuaweiId> OnSignInSuccess { get; set; }
        public Action<HMSException> OnSignInFailed { get; set; }

        private HuaweiIdAuthService _authService;

        private HuaweiIdAuthService authService
        {
            get
            {
                if (_authService == null)
                {
                    _authService = DefaultAuthService;
                }
                return _authService;
            }
        }

        // Start is called before the first frame update
        // void Start()
        // {
        //     authService = DefaultAuthService;
        // }

        public void SignIn()
        {
            authService.StartSignIn((authId) =>
            {
                HuaweiId = authId;
                OnSignInSuccess?.Invoke(authId);
            }, (error) =>
            {
                HuaweiId = null;
                OnSignInFailed?.Invoke(error);
            });
        }

        public void SignOut()
        {
            authService.SignOut();
            HuaweiId = null;
        }
    }
}
