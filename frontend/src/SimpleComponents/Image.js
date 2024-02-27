import React from 'react'
import PropTypes from 'prop-types'
import cs from 'classnames'

import '../css/image.css'

const Image = ({
    src, alt, className, width, height, circle, ...attrs
               }) => {

    if (!src) {
        src = `https://via.placeholder.com/${width}x${height}`
    }
    const classes = cs (
        className,
        {circle}
    )

    return (
        <img
        src = {src}
        alt = {alt}
        className={classes}
        width={width}
        height={height}
        {...attrs}
        />
    );
};

Image.propTypes = {
    src: PropTypes.string,
    alt: PropTypes.string,
    className: PropTypes.string,
    width: PropTypes.string,
    height: PropTypes.string,
    circle: PropTypes.bool,
};

Image.defaultProps = {
    src: '',
    alt: 'image',
    className: '',
    width: '100%',
    height: '100%',
    circle: false,
};

export default Image;