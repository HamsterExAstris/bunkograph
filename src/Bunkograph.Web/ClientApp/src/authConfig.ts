export const msalConfig = {
  auth: {
    clientId: "57201681-1ee9-4fbf-9fd4-50a5166f8896",
    authority: "https://login.microsoftonline.com/consumers", // This is a URL (e.g. https://login.microsoftonline.com/{your tenant ID})
    redirectUri: "https://localhost:44405",
  },
  cache: {
    cacheLocation: "sessionStorage", // This configures where your cache will be stored
    storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
  }
};

// Add scopes here for ID token to be used at Microsoft identity platform endpoints.
export const loginRequest = {
  scopes: ["User.Read"]
};

// Add the endpoints here for Microsoft Graph API services you'd like to use.
export const graphConfig = {
  graphMeEndpoint: "https://graph.microsoft.com/v1.0/me"
};