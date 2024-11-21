import {useAppContext} from '../providers/AppProvider'

const useAuth = () => {
    const {state, dispatch} = useAppContext();
    const isAuthenticated = state.accessToken !== undefined;
    const signIn = (accessToken: string) => {
        dispatch({type: 'SIGN_IN', accessToken: accessToken});
    }
    const signOut = () => {
        dispatch({type: 'SIGN_OUT'});
    }
    return {isAuthenticated, signIn, signOut, state};
}

export default useAuth;