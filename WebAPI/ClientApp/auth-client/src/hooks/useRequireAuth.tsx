import { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import useAuth from './useAuth';

const useRequireAuth = () => {
    const { isAuthenticated } = useAuth();
    const history = useHistory();
    
    useEffect(() => {
        if (!isAuthenticated) {
        history.push('/login');
        //return (<p>Unauthorized</p>);
        }
    }, [isAuthenticated, history]);
    
    return isAuthenticated;
}

export default useRequireAuth;