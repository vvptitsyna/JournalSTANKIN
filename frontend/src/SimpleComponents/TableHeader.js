const TableHeader = () => {

    return (
        <div className="table">
            <div className="table__row table__row--header">
                <div className="table__cell">ФИО</div>
                <div className="table__cell">М1</div>
                <div className="table__cell">М2</div>
                <div className="table__cell">Экзамен/Зачет</div>
                <div className="table__cell">Курсовая работа</div>
            </div>
        </div>
    )
};

export default TableHeader;