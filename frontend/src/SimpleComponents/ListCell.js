import React from "react";
import PropTypes from "prop-types";
import ListRow from "./ListRow";

const ListCell = ({child, value, ...attrs}) => {

    return (
        <div className="list__cell">
            <p className={'p_cell' + (value === '' || value === 0 || value === null ? 'no-value' : '')}>
                {value === '' || value === 0 || value === null ? 'нет' : value}
            </p>
        </div>
    );
};

ListCell.propTypes = {
    child: PropTypes.any,
    value: PropTypes.oneOfType([PropTypes.string, PropTypes.number])
}

ListCell.defaultProps = {
    child: [],
    value: '',
}

export default ListCell;