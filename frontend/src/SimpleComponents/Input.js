import React from 'react'
import PropTypes from 'prop-types'
import cs from 'classnames'

import '../css/input.css'

const Input = ({
    id, className,label, error, ...attrs
               }) => {

    const classes = cs(
        'input',
        className,
        {error}
    )
    return (
        <div className="input-wrapper">

            {attrs.required &&
                <span className="input-required">Обязательное поле.</span>
            }
            <input
            name={id}
            id={id}
            className={classes}
            {...attrs}
            />
            {label &&
                <label className="input-label" htmlFor={id}> {label} </label>
            }
            {error &&
                <span className="input-error">{error}</span>
            }

        </div>
    );
};

Input.propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    label: PropTypes.string,
    error: PropTypes.string,
};
Input.defaultProps = {
    className: "",
    label: "",
    error: "",
};
export default Input;