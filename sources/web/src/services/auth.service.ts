import { email } from "zod";
import { authApiV1 } from "../api/auth.api";
import { useAuthStore } from "../store/auth.store";

export async function login(email: string, password: string) {
  const res = await authApiV1.post("/login", { email, password });

  useAuthStore.getState().setAccessToken(res.data.value.accessToken);
  useAuthStore.getState().setCurrentUser(res.data.value.currentUser);

  localStorage.setItem("currentUser", JSON.stringify(res.data.value.currentUser));
}

export async function logout() {
  await authApiV1.post("/logout", {
    email: useAuthStore.getState().currentUser?.email,
  });
  useAuthStore.getState().logout();

  localStorage.removeItem("currentUser");
}

export async function loadSessionOnInit() {

  const { setAccessToken, setCurrentUser, setAuthLoaded, logout } = useAuthStore.getState();
  const storedUser = localStorage.getItem("currentUser");

  if (storedUser) {
    setCurrentUser(JSON.parse(storedUser));
  }

  try {
    if (storedUser) {
      const res = await authApiV1.post("/refresh-token", {
        email: JSON.parse(storedUser).email,
      });
      setAccessToken(res.data.value.accessToken);
      setCurrentUser(res.data.value.currentUser);
    }
  } catch (error) {
    logout();
  } finally {
    setAuthLoaded(true);
  }
}

export async function getCurrentUser() {
  return await authApiV1.get("/");
}
