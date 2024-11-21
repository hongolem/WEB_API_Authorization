import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import useAuth from './useAuth';

const useRequireAuth = () => {
    const { isAuthenticated } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!isAuthenticated) {
            navigate('/login');
            //return (<p>Unauthorized</p>);
        }
    }, [isAuthenticated, history]);

    return isAuthenticated;
}

export default useRequireAuth;