import React from "react";
import ListCell from "./ListCell";

const ListRow = ({children, fieldsToShow,onRowClick, ...attrs}) => {



    const fields = Object.entries(children)
        .filter(([key]) => fieldsToShow.includes(key))
        .map(([key, value]) => ({ name: key, value }));

    const handleRowClick = () => {
        if (onRowClick) {
            onRowClick(attrs.id);
        }
    };

    return (
        <div className="list__row " id={attrs.id} onClick={() => handleRowClick()}>
            {
                fields.map((field) => (
                    <ListCell fieldName={field.name} value={field.value} />))
            }
        </div>
    );
};

export default ListRow;