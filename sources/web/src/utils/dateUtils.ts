export function formatPostDate(dateString?: string): string {
    if (!dateString) return "";

    const date = new Date(dateString);
    if (isNaN(date.getTime())) return "";

    const now = new Date();
    const diffTime = Math.abs(now.getTime() - date.getTime());
    const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));
    
    // Format Month Day (e.g. Dec 19)
    const options: Intl.DateTimeFormatOptions = { month: 'short', day: 'numeric' };
    let formattedDate = date.toLocaleDateString('en-US', options);
    
    // If it's a different year, append the year (e.g. Dec 19, 2023)
    if (date.getFullYear() !== now.getFullYear()) {
        const yearOptions: Intl.DateTimeFormatOptions = { year: '2-digit' };
        formattedDate += ` '${date.toLocaleDateString('en-US', yearOptions)}`;
    }
    
    // Format the "Relative" part
    let relative = "";
    if (diffDays === 0) {
        // Less than a day
        const diffHours = Math.floor(diffTime / (1000 * 60 * 60));
        if (diffHours === 0) {
            const diffMinutes = Math.floor(diffTime / (1000 * 60));
            relative = diffMinutes <= 1 ? "just now" : `${diffMinutes} mins ago`;
        } else {
            relative = diffHours === 1 ? "1 hour ago" : `${diffHours} hours ago`;
        }
    } else if (diffDays === 1) {
        relative = "yesterday";
    } else if (diffDays < 30) {
        relative = `${diffDays} days ago`;
    } else if (diffDays < 365) {
        const diffMonths = Math.floor(diffDays / 30);
        relative = diffMonths === 1 ? "1 month ago" : `${diffMonths} months ago`;
    } else {
        const diffYears = Math.floor(diffDays / 365);
        relative = diffYears === 1 ? "1 year ago" : `${diffYears} years ago`;
    }

    return `${formattedDate} (${relative})`;
}
