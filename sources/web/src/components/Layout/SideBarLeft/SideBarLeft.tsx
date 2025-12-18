import { Link } from "@tanstack/react-router";
import MenuHouse from '@/assets/menu_house.png'
import MenuTulips from "@/assets/menu_tulips.png"
import MenuFileStorage from "@/assets/menu_file_storage.png"
import MenuGraduation from "@/assets/menu_graduation.png"
import MenuHeart from "@/assets/menu_heart.png"
import MenuLightbuld from "@/assets/menu_lightbulb.png"
import MenuMicrophone from "@/assets/menu_microphone.png"
import MenuPrograming from "@/assets/menu_programming.png"
import MenuVideoCamera from "@/assets/menu_video_camera.png"
import MenuVideoAward from "@/assets/menu_award.png"
import MenuFaceSmile from "@/assets/menu_face_smile.png"
import MenuSpark from "@/assets/menu_spark.png"
import MenuContact from "@/assets/menu_contact.png"
import MenuTree from "@/assets/menu_tree.png"
import MenuFlowerPot from "@/assets/menu_flower_pot.png"
import MenuLike from "@/assets/menu_like.png"

function SideBarLeft() {
    return (
        <div className="w-full">
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuHouse}></img>
                <span className="text-[15px] font-light">Home</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuPrograming}></img>
                <span className="text-[15px] font-light">DEV++</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuFileStorage}></img>
                <span className="text-[15px] font-light">Reading List</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuMicrophone}></img>
                <span className="text-[15px] font-light">Podcasts</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuVideoCamera}></img>
                <span className="text-[15px] font-light">Videos</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuGraduation}></img>
                <span className="text-[15px] font-light">DEV Education Tracks</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuVideoAward}></img>
                <span className="text-[15px] font-light">DEV Challenges</span>
            </Link>
             <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuLightbuld}></img>
                <span className="text-[15px] font-light">DEV Help</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuHeart}></img>
                <span className="text-[15px] font-light">Advertise on DEV</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuSpark}></img>
                <span className="text-[15px] font-light">DEV Showcase</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuFaceSmile}></img>
                <span className="text-[15px] font-light">About</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuContact}></img>
                <span className="text-[15px] font-light">Contact</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuTulips}></img>
                <span className="text-[15px] font-light">Forem shop</span>
            </Link>
 
            <h3 className="px-2 mt-4 mb-2 text-base font-bold text-[#242424]">Other</h3>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuTree}></img>
                <span className="text-[15px] font-light">Code of Conduct</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuFlowerPot}></img>
                <span className="text-[15px] font-light">Privacy Policy</span>
            </Link>
            <Link to='/' className="w-full flex items-center px-2 py-2 gap-0 rounded-md hover:bg-primary/10 hover:underline hover:text-primary mb-2">
                <img className="w-6 h-6 mr-2" src={MenuLike}></img>
                <span className="text-[15px] font-light">Terms of Use</span>
            </Link>
        </div>
    )
}

export default SideBarLeft;