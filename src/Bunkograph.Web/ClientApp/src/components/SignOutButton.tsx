import { IPublicClientApplication } from "@azure/msal-browser";
import { useMsal } from "@azure/msal-react";
import { Button } from "reactstrap";

function handleLogout(instance: IPublicClientApplication) {
  instance.logoutPopup().catch(e => {
    console.error(e);
  });
}

/**
 * Renders a button which, when selected, will open a popup for logout
 */
export const SignOutButton = () => {
  const { instance } = useMsal();

  return (
    <Button variant="secondary" className="ml-auto" onClick={() => handleLogout(instance)}>Sign out using Popup</Button>
  );
}