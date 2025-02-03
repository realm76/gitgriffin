import { createSignal } from "solid-js";
import {setUser, user} from "../stores/user-store";

type AccessToken = Readonly<{}>

const postLogin = async (username: string, password: string) => {
    const response = await fetch(`${import.meta.env.API_URL}/`);

    return (await response.json()) as AccessToken;
}


const Login = () => {
    const [email, setEmail] = createSignal("");
    const [password, setPassword] = createSignal("");
    const [error, setError] = createSignal("");

    const handleLogin = async () => {
        setError("");

        if (!email() || !password()) {
            setError("Please enter email and password.");
            return;
        }

        // Simulate authentication (replace with real API call)
        if (email() === "test@example.com" && password() === "password123") {
            const userData = { email: email() };
            setUser(userData);
            localStorage.setItem("user", JSON.stringify(userData));
        } else {
            setError("Invalid credentials.");
        }
    };

    const handleLogout = () => {
        setUser(null);
        localStorage.removeItem("user");
    };

    return (
        <div class="login-container">
            {user() ? (
                <div>
                    <p>Welcome, {user()?.email}!</p>
                    <button onClick={handleLogout}>Logout</button>
                </div>
            ) : (
                <div>
                    <h2>Login</h2>
                    {error() && <p class="error">{error()}</p>}
                    <input
                        type="email"
                        placeholder="Email"
                        value={email()}
                        onInput={(e) => setEmail(e.currentTarget.value)}
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password()}
                        onInput={(e) => setPassword(e.currentTarget.value)}
                    />
                    <button onClick={handleLogin}>Login</button>
                </div>
            )}
        </div>
    );
};

export { Login };
