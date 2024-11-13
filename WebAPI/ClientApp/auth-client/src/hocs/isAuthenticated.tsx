import { useAppContext } from "../providers/AppProvider";

export default function isAuthenticated<T extends object>(Component: React.ComponentType<T>) {
    return function isAuthenticated(props: T & React.Attributes) {
        const { state } = useAppContext();
        const isAuthenticated = state.accessToken !== undefined;
        if (isAuthenticated) {
            return <Component {...props} />;
        }
        return <div>Not Authenticated</div>;
    }
}