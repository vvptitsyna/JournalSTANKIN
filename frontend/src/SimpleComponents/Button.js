import React from 'react'
import PropTypes from 'prop-types'
import cs from 'classnames'

import '../css/button.css'

const Button = ({
    children, onClick, className, disabled, active, ...attrs
                }) => {

    const classes = cs(
        'btn',
        className,
        {disabled},
        {active}
    )
    return (
    <button
        className = {classes}
        disable = {disabled}
        onClick = {onClick}
        {...attrs}
    >{children}
    </button>
    );
};
Button.propTypes = {
    children: PropTypes.node,
    onClick: PropTypes.func,
    className: PropTypes.string,
    disabled: PropTypes.bool,
    active: PropTypes.bool,
};

Button.defaultProps = {
    children: '',
    onClick: () => {},
    className: '',
    disabled: false,
    active: false,

};
export default Button;