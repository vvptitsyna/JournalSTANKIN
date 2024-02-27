import React from "react";

import '../css/profileNav.css'
import Image from "../SimpleComponents/Image";
const ProfileNav = () => {

    return(
        <div className="profile-nav">
            <p>Новоселова Ольга Вячеславовна</p>
            <Image width="30" height="30" circle="true"/>
        </div>
    );
};

export default ProfileNav;