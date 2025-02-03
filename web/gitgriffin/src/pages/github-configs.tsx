import {createResource, Show} from "solid-js";

type GithubConfig = Readonly<{
    accessToken: string;
}>;

const fetchGithubConfig = async () => {
    const response = await fetch(`${import.meta.env.API_URL}/configs/github`);

    return (await response.json()) as GithubConfig;
}

export function GithubConfigs() {
    const [config] = createResource(fetchGithubConfig);

    return (
        <div>
            <h1>Github Config</h1>

            <Show when={config()}>
                <label>
                    Access Token
                    <input type={"text"} value={config()?.accessToken} />
                </label>
            </Show>
        </div>
    )
}
