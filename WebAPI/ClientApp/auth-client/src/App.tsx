import './App.css'
import {useEffect, useRef, useState} from "react";
import useAuth from "./hooks/useAuth.tsx";

interface WeatherForecast {
    date: string,
    summary: string,
    temperatureC: number,
    temperatureF: number,
};

function App() {
    const {isAuthenticated, signIn: signInAccessToken, signOut, state} = useAuth();
    const username = useRef<HTMLInputElement>(null);
    const password = useRef<HTMLInputElement>(null);
    const [weather, setWeather] = useState([] as WeatherForecast[]);

    const signIn = () => {
        fetch("http://localhost:5220/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({username: username.current?.value, password: password.current?.value}),
        }).then(response => {
            if (response.ok) {
                return response.json();
            }
        }).then(data => {
            if (data) {
                signInAccessToken(data.value);
            }
        }).catch(error => {
            console.error(error);
        });
    }

    useEffect(() => {
        if (!isAuthenticated) return;

        fetch("http://localhost:5281/WeatherForecast", {
            headers: {
                "Authorization": `Bearer ${state.accessToken}`,
            },
        }).then(response => {
            if (response.ok) {
                return response.json();
            }
        }).then(data => {
            setWeather(data as WeatherForecast[]);
        }).catch(error => {
            console.error(error);
        });
    }, [isAuthenticated]);

    return (
        <>
            {isAuthenticated ? (
                <div>
                    <button onClick={signOut}>Sign Out</button>
                    <div>{weather.map(x =>
                        <div><hr/>
                            <p>{x.date}</p>
                            <p>{x.summary}</p>
                            <p>{x.temperatureC}</p>
                            <p>{x.temperatureF}</p>
                        </div>
                    )}</div>
                </div>
            ) : (
                <>
                    <input type={"text"} ref={username} placeholder={"Username"}/>
                    <input type={"password"} ref={password} placeholder={"Password"}/>
                    <button onClick={() => signIn()}>Sign In</button>
                </>
            )}
        </>
    )
}

export default App
