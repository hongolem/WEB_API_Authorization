import React, {createContext, useReducer, FC, PropsWithChildren, useContext} from 'react';

type ApplicationState = {
    accessToken?: string,
    userId?: string,
    username?: string,
}

export const SET_ACCESS_TOKEN = 'SET_ACCESS_TOKEN';
export const CLEAR_ACCESS_TOKEN = 'CLEAR_ACCESS_TOKEN';
export const SIGN_IN = 'SIGN_IN';
export const SIGN_OUT = 'SIGN_OUT';

export type ApplicationAction = 
    {type: 'SET_ACCESS_TOKEN', accessToken: string} |
    {type: 'CLEAR_ACCESS_TOKEN'} |
    {type: 'SIGN_IN', accessToken: string} |
    {type: 'SIGN_OUT'};

const applicationReducer = (state: ApplicationState, action: ApplicationAction): ApplicationState => {
    switch (action.type) {
        case SET_ACCESS_TOKEN:
            return {...state, accessToken: action.accessToken};
        case CLEAR_ACCESS_TOKEN:
            return {...state, accessToken: undefined};
        case SIGN_IN: {
            const decodedToken = decodeJwt(action.accessToken);
            const userId = decodedToken.sub || decodedToken.user_id;
            const username = decodedToken.username;
            return {...state, userId: userId, username: username, accessToken: action.accessToken};
        }
        case SIGN_OUT:
            return {...state, userId: undefined, username: undefined, accessToken: undefined};
        default:
            return state;
    }
}

const initialState: ApplicationState = {
    accessToken: undefined,
}

type ApplicationContextType = {
    state: ApplicationState,
    dispatch: React.Dispatch<ApplicationAction>
}

export const ApplicationContext = createContext<ApplicationContextType>({state: initialState, dispatch: () => {}});

const ApplicationProvider:FC<PropsWithChildren> = ({children}) => {
    const [state, dispatch] = useReducer(applicationReducer, initialState);
    return (
        <ApplicationContext.Provider value={{state, dispatch}}>
            {children}
        </ApplicationContext.Provider>
    );
}

export const useAppContext = () => {
    return useContext(ApplicationContext);
}

export default ApplicationProvider;

const decodeJwt = (token: string) => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
        atob(base64)
            .split('')
            .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
            .join('')
    );
    return JSON.parse(jsonPayload);
}