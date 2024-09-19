import {useState} from 'react'
import 'bulma/css/bulma.css'

function App() {

    const [artworks, setArtworks] = useState([]);
    const [showModal, setShowModal] = useState(false);

    const listItems = artworks.map((artwork, index) => <li key={index} className="block">
        <a href={artwork.link} target="_blank">
            {artwork.title}
        </a>
    </li>);

    function fetchArtworks() {
        fetch('/api/v1/artworks')
            .then(response => response.json())
            .then(data => {
                console.log(data)
                setArtworks(prevState => prevState = data);
            });
    }

    return (<>
        <div className="m-1">
        <button onClick={() => fetchArtworks()} className="button">add</button>
        <ol>
            {listItems}
        </ol>
        <button onClick={() => setShowModal(true)} className="button">关于</button>
        <div className={"modal " + (showModal ? 'is-active' : '')}>
            <div className="modal-background"></div>
            <div className="modal-content">
                <div className="box">
                    <h1>关于</h1>
                    <p>cos收集站</p>
                </div>
            </div>
            <button onClick={() => setShowModal(false)} className="modal-close is-large" aria-label="close"></button>
        </div>
        </div>
    </>)
}

export default App
