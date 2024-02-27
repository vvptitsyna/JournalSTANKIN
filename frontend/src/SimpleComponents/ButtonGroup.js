import react from "react"
import cs from "classnames";

import '../css/carousel.css'
import PropTypes from "prop-types";
const ButtonGroup = ({
    children, className,vertical, ...attrs
                     }) => {

    const classes = cs(
        'btn-group',
        className,
        {vertical}
    )

    return(
        <div
            className={classes}
            {...attrs}
        >
            {children}
        </div>
    )
}

ButtonGroup.propTypes = {
    className: PropTypes.string,
    children: PropTypes.node,
    vertical: PropTypes.bool,
};

ButtonGroup.defaultProps = {
    className: '',
    children: null,
    vertical: false,
};

export default ButtonGroup;