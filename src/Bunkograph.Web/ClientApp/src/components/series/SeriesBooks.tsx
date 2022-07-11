import { AccountInfo, AuthenticationResult, InteractionRequiredAuthError, IPublicClientApplication } from "@azure/msal-browser";
import { useMsal } from "@azure/msal-react";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { Col, Form, Row } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { loginRequest } from "../../authConfig";

async function populateSeriesData(seriesId: number) {
  const response = await fetch('api/series/volumes/' + seriesId);
  const data = await response.json() as ISeries;

  for (const book of data.books) {
    for (const language of languages) {
      const edition = book.editions[language];
      if (edition) {
        fixDates(edition, ["releaseDate"]);
      }
    }
  }
  return data as ISeries;
}

const fixDates = (item: Record<string, any>, dateFields: string[]) => {
  dateFields.forEach(dateField => {
    if (item[dateField]) {
      item[dateField] = dayjs(item[dateField]).toDate();
    }
  });
};

type Language = "jp" | "en" | "ab"

interface IBookEdition {
  publisherId?: number
  releaseDate?: Date
}

interface IBook {
  bookId: number
  editions: Record<Language, IBookEdition>
}

interface ISeries {
  seriesId: number
  books: IBook[]
}

const languages: Language[] = ["jp", "en", "ab"]

async function saveForm(instance: IPublicClientApplication, accounts: AccountInfo[], series: ISeries) {
  const request = {
    ...loginRequest,
    account: accounts[0]
  };

  let tokenResponse: AuthenticationResult | undefined;
  try {
    tokenResponse = await instance.acquireTokenSilent(request);
  }
  catch (error) {
    if (error instanceof InteractionRequiredAuthError) {
      tokenResponse = await instance.acquireTokenPopup(request);
    } else {
      throw error;
    }
  }
  const idToken = tokenResponse?.idToken;

  const response = await fetch('api/series/volumes/' + series.seriesId, {
    method: "POST",
    headers: {
      'Authorization': "Bearer " + idToken ?? ""
    },
    body: JSON.stringify(series)
  });
  if (!response.ok) {
    // StatusText is not settable on ASP.NET Core - omit it?
    throw new Error("Request failed: " + response.status + " " + response.statusText + " " + await response.text());
  }
}

const SeriesBooks: React.FC = () => {
  const [seriesInfo, setSeriesInfo] = useState<ISeries | undefined>();
  const { instance, accounts } = useMsal();

  const navigate = useNavigate();
  const params = useParams();

  useEffect(() => {
    const getAnswer = async () => {
      // If the URL specifies a specific series, load the data into state for rendering.
      const seriesId = params.seriesId ? parseInt(params.seriesId) : undefined;
      if (seriesId) {
        // Get the series data from the API.
        const series = await populateSeriesData(seriesId);

        // Stub out missing languages.
        for (const book of series.books) {
          for (const language of languages) {
            if (!book.editions[language]) {
              book.editions[language] = {} as IBookEdition;
            }
          }
        }
        setSeriesInfo(series);
      }
      else {
        setSeriesInfo(undefined);
      }
    }
    getAnswer();
  }, [params.seriesId]);

  const handleSubmit = async () => {
    if (seriesInfo) {
      await saveForm(instance, accounts, seriesInfo);
    }
    navigate('/series');
  }

  return (
    <>
      {
        seriesInfo &&
        <Form onSubmit={handleSubmit}>
          <Form.Group className="mb-3">
            <Form.Label>SeriesId</Form.Label>
            <Form.Control plaintext readOnly defaultValue={seriesInfo.seriesId} />
          </Form.Group>
          {
            seriesInfo.books.map((book) =>
              <>
                <hr />
                <Form.Group key={book.bookId} className="mb-3">
                  <Form.Label>BookId</Form.Label>
                  <Form.Control plaintext readOnly defaultValue={book.bookId} />
                </Form.Group>
                {
                  languages.map((language) =>
                    <Row className="mb-3" key={language}>
                      <>
                        <Form.Group as={Col}>
                          <Form.Label>Language</Form.Label>
                          <Form.Control plaintext readOnly defaultValue={language} />
                        </Form.Group>
                        <Form.Group as={Col}>
                          <Form.Label>PublisherId</Form.Label>
                          <Form.Control type="number" step="1" defaultValue={book.editions[language].publisherId} />
                        </Form.Group>
                        <Form.Group as={Col}>
                          <Form.Label>Language</Form.Label>
                          <Form.Control type="date" defaultValue={book.editions[language].releaseDate?.toISOString()?.substring(0, 10)} />
                        </Form.Group>
                      </>
                    </Row>
                  )
                }
              </>
            )
          }
        </Form>
      }
    </>
  )
}

export default SeriesBooks;
