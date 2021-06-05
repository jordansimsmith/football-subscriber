import jwtDecode from 'jwt-decode';

let accessToken: string;

export async function getAccessToken(): Promise<string> {
  if (accessToken) {
    const { exp } = jwtDecode<{ exp: number }>(accessToken);

    const expiryTimeMs = exp * 1000 - 60000; // account for latency
    if (Date.now() < expiryTimeMs) {
      return accessToken;
    } else {
      console.log('access token expired, fetching from server');
    }
  } else {
    console.log('access token not found, fetching from server');
  }

  const url = '/api/auth/token';
  const res = await fetch(url);
  const data = await res.json();

  accessToken = data.accessToken;

  return accessToken;
}
