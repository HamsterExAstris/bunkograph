export const msalConfig = {
  auth: {
    clientId: "6ea66498-a20d-46e5-822c-a767f059fb02",
    authority: "https://login.microsoftonline.com/abb1108c-49cc-4f42-85d5-50dec480ae44", // This is a URL (e.g. https://login.microsoftonline.com/{your tenant ID})
    // redirectUri MUST be specified, as it must match exactly what's in the app registration. If it isn't, MSAL uses the current URL, and subpages don't match.
    redirectUri: window.location.origin
  },
  cache: {
    cacheLocation: "localStorage", // This configures where your cache will be stored
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