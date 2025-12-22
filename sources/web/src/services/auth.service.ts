import { authApiV1 } from "../api/auth.api";
import { useAuthStore } from "../store/auth.store";

export async function login(email: string, password: string) {
  const res = await authApiV1.post("/login", { email, password });

  useAuthStore.getState().setAccessToken(res.data.value.accessToken);
  useAuthStore.getState().setCurrentUser(res.data.value.currentUser);
}

export async function logout() {
  await authApiV1.post("/logout");
  useAuthStore.getState().logout();
}

export async function loadSessionOnInit() {

  const {setAccessToken, setCurrentUser, setAuthLoaded, logout} = useAuthStore.getState();

  try {
    const res = await authApiV1.post("/refresh-token");
    setAccessToken(res.data.value.accessToken);
    setCurrentUser(res.data.value.currentUser);
  } catch {
    logout();
  } finally {
    setAuthLoaded(true);
  }
}

export async function getCurrentUser() {
  return await authApiV1.get("/");
}
