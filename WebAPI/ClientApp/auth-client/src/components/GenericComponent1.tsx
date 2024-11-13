import isAuthenticated from '../hocs/isAuthenticated';

const GenericComponent1: React.FC = () => {
    return (
        <div>
            <h1>GenericComponent</h1>
        </div>
    );
}

export default isAuthenticated(GenericComponent1);