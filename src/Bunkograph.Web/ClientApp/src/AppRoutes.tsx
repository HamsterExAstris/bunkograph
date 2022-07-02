import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import GraphSample from "./components/GraphSample";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/graphsample',
        element: <GraphSample series="The Saga of Tanya the Evil" publisher="Yen Press" />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
];

export default AppRoutes;