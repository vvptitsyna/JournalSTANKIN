import React from 'react'
import PropTypes from 'prop-types'
import cs from 'classnames'

//import '../css/logo.css'

const Logo = ({
    width, height, ...attrs
              }) => {
    return (
        <img
        src = './img/Logo.svg'
        width={width}
        height={height}
        {...attrs}
        />
    );
};

Logo.propTypes = {
    width: PropTypes.string,
    height: PropTypes.string,
}

Logo.defaultProps = {
    width: '305px',
    height: '200px',
}
export default Logo;