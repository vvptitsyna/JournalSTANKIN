
    import React from "react";
    import ListRow from "./ListRow";
    import ListHeader from "./ListHeader";
    import '../css/list.css'
    import classNames from "classnames";

    const List = ({children, fieldsToShow,fieldsHeader, onRowClick,className, ...attrs}) => {

        const classes = classNames(
            'list',
            className,

        )
        return (
                <div className={classes}>
                    <ListHeader fieldsHeader={fieldsHeader}/>
                    {children.map((child) => {
                        return (<ListRow id={child.id} children={child} fieldsToShow={fieldsToShow} onRowClick={onRowClick}/>)
                    })}
            </div>
        );
    };

    export default List;