import useAuth from '../hooks/useAuth';

const ShowState = () => {
    const {isAuthenticated} = useAuth();
    if (isAuthenticated) {
        return <div>Authenticated</div>;
    }
    return <div>Not Authenticated</div>;
}

export default ShowState;