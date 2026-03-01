import { commandApiV1 } from "../api/command.api";

export async function UploadImage(file: File) {
    const formData = new FormData();
    formData.append("file", file);

    return await commandApiV1.post<{ secure_url: string }>("/files/image", formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
}
