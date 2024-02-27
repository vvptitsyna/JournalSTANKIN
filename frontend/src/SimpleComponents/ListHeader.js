

const ListHeader = ({fieldsHeader}) => {

    return (
            <div className="list__row list__row--header">
                {fieldsHeader.map((field) => (
                    <div className="list__cell header" key={field}>
                        <p>{field}</p>
                    </div>
                ))}
        </div>
    )
};

export default ListHeader;