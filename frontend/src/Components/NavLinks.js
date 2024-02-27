import React from "react";
import {Link} from "react-router-dom";

import '../css/navLinks.css'

const NavLinks = () => {

    return(
        <div className="navLinks">
            <Link to="/main">предметы</Link>
        </div>
    );
};

export default NavLinks;