import React from "react";
import Logo from "../SimpleComponents/Logo";
import { Link } from "react-router-dom";
import ProfileNav from "../Components/ProfileNav";
import NavLinks from "../Components/NavLinks";

import '../css/navigation.css'

const Navigation = () => {
    return (
        <div className="navigation-bar">
            <Logo height="50px" width="100px"/>
            <NavLinks/>
            <ProfileNav/>
        </div>
    );
};
export default Navigation;