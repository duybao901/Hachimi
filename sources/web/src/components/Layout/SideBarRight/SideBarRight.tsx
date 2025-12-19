import { Link } from "@tanstack/react-router";

function SideBarRight() {
    return (
        <div className="w-full rounded-md bg-white">
            <h2 className="py-4 px-4 text-lg font-semibold">Active discussions</h2>
            <div className="">
                <Link to='/' className="group block py-4 px-4 border-t border-t-gray-100">
                    <p className="group-hover:text-primary text-sm text-(--link-color)">I Made My Laravel API 83% Faster by Rethinking Database Queries</p>
                    <span className="text-sm font-light text-(--link-color-secondary)">6 comments</span>
                </Link>
                <Link to='/' className="group block py-4 px-4 border-t border-t-gray-100">
                    <p className="group-hover:text-primary text-sm text-(--link-color)">Welcome Thread - v356 🚀</p>
                    <span className="text-sm font-light text-(--link-color-secondary)">23 comments</span>
                </Link>
                <Link to='/' className="group block py-4 px-4 border-t border-t-gray-100">
                    <p className="group-hover:text-primary text-sm text-(--link-color)">Linux Without Fanboyism: An Honest Developer’s Perspective</p>
                    <span className="text-sm font-light text-(--link-color-secondary)">3comments</span>
                </Link>
            </div>
        </div>
    )
}

export default SideBarRight;