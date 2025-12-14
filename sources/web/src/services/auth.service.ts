import { authApiV1 } from "../api/auth.api";
import { useAuthStore } from "../store/auth.store";
import { queryApi } from "../api/query.api";

export async function login(email: string, password: string) {
  const res = await authApiV1.post("/login", { email, password });

  useAuthStore.getState().setAccessToken(res.data.value.accessToken);

  // const profile = await queryApi.get("/me");
  // useAuthStore.getState().setUser(profile.data);
}

export async function logout() {
  await authApiV1.post("/logout");
  useAuthStore.getState().logout();
}

export async function loadSessionOnInit() {
  try {
    const res = await authApiV1.post("/refresh-token");
    useAuthStore.getState().setAccessToken(res.data.accessToken);

    // const profile = await queryApi.get("/me");
    // useAuthStore.getState().setUser(profile.data);
  } catch {
    useAuthStore.getState().logout();
  }
}

export async function getCurrentUser() {
  return await authApiV1.get("/");
}