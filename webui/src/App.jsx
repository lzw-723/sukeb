import {useState} from 'react'
import 'bulma/css/bulma.css'
import ArtworkCard from "./compents/ArtworkCard.jsx";

function App() {

    const [artworks, setArtworks] = useState([]);
    const [showModal, setShowModal] = useState(false);

    const listItems = artworks.map((artwork, index) => <div key={index} className="cell block">
        <ArtworkCard artwork={artwork}/>
    </div>);

    function fetchArtworks() {
        fetch('/api/v1/artworks')
            .then(response => response.json())
            .then(data => {
                console.log(data)
                setArtworks(prevState => prevState = data);
            });
    }

    fetchArtworks();
    return (<>
        <div style={{width: "100vw"}}>
            <button onClick={() => fetchArtworks()} className="button">刷新</button>
            <div className="fixed-grid has-1-cols-mobile has-2-cols-tablet has-4-cols-desktop">
                <div className="grid">
                    {listItems}
                </div>
            </div>
            <button onClick={() => setShowModal(true)} className="button">关于</button>
            <div className={"modal " + (showModal ? 'is-active' : '')}>
                <div className="modal-background"></div>
                <div className="modal-content">
                    <div className="box">
                        <h1>关于</h1>
                        <p>cos收集站</p>
                    </div>
                </div>
                <button onClick={() => setShowModal(false)} className="modal-close is-large"
                        aria-label="close"></button>
            </div>
        </div>
    </>)
}

export default App
