import { AccountInfo, AuthenticationResult, InteractionRequiredAuthError, IPublicClientApplication } from "@azure/msal-browser";
import { useMsal } from "@azure/msal-react";
import { useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { loginRequest } from "../authConfig";
import GraphSample from "./BookSeriesGraph";

export interface ISeriesInfo {
  seriesId: number,
  englishName: string,
  publisher?: string
}

async function populateSeriesData(instance: IPublicClientApplication, accounts: AccountInfo[]) {
  const request = {
    ...loginRequest,
    account: accounts[0]
  };

  let tokenResponse: AuthenticationResult | undefined;
  try {
    tokenResponse = await instance.acquireTokenSilent(request);
  }
  catch (error)
  {
    if (error instanceof InteractionRequiredAuthError) {
      tokenResponse = await instance.acquireTokenPopup(request);
    } else {
      throw error;
    }
  }
  const idToken = tokenResponse?.idToken;

  const response = await fetch('api/series', {
    headers: {
      'Authorization': "Bearer " + idToken ?? ""
    }
  });
  const data = await response.json();

  return data as ISeriesInfo[];
}

const Series: React.FC = () => {
  const { instance, accounts } = useMsal();
  const [seriesInfos, setSeriesInfos] = useState<ISeriesInfo[]>();
  const [seriesInfo, setSeriesInfo] = useState<ISeriesInfo | undefined>();

  const navigate = useNavigate();
  const params = useParams();

  const setSeries = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const seriesId = parseInt(e.target.value);
    if (seriesId) {
      navigate('/series/' + seriesId);
    }
    else {
      navigate('/series');
    }
  }

  useEffect(() => {
    const getAnswer = async () => {
      // Get the data for all series from the API.
      const series = await populateSeriesData(instance, accounts);
      setSeriesInfos(series);

      // If the URL specifies a specific series, load the data into state for rendering.
      const seriesId = params.seriesId ? parseInt(params.seriesId) : undefined;
      if (seriesId) {
        for (const seriesInfo of series) {
          if (seriesInfo.seriesId === seriesId) {
            setSeriesInfo(seriesInfo);
          }
        }
      }
      else {
        setSeriesInfo(undefined);
      }
    }
    getAnswer();
  }, [params.seriesId, instance, accounts]);

  return (
    <>
      {
        seriesInfos &&
        <Form.Select aria-label="Default select example" onChange={setSeries}>
            {
              !seriesInfo && <option value={0}>Please select a series</option>
            }
            {
              seriesInfos.map((value: ISeriesInfo) => <option key={value.seriesId} value={value.seriesId}>{value.englishName}</option>)
            }
          </Form.Select>
      }
      {
        seriesInfo && <GraphSample seriesInfo={seriesInfo} />
      }
    </>
  )
}

export default Series;
