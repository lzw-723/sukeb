import {useEffect, useState} from 'react'
import 'bulma/css/bulma.css'
import ArtworkCard from "./compents/ArtworkCard.jsx";

function App() {

    const [artworks, setArtworks] = useState([]);
    const [showModal, setShowModal] = useState(false);

    const listItems = artworks.map((artwork, index) => <div key={index} className="cell block">
        <ArtworkCard artwork={artwork}/>
    </div>);

    function fetchArtworks() {
        return fetch('/api/v1/artworks')
            .then(response => response.json())
            .then(data => {
                setArtworks(data);
                console.log(data)
            });
    }

    useEffect(() => {
        fetchArtworks();
    }, [])
    return (<>
        <section className="hero">
            <div className="hero-body">
                <p className="title">sukeb</p>
                <p className="subtitle">cos收集站</p>
            </div>
        </section>
        <div className="container p-1">
            <button onClick={() => fetchArtworks()} className="button">刷新</button>
            <div className="fixed-grid has-1-cols-mobile has-2-cols-tablet has-4-cols-desktop">
                <div className="grid">
                    {listItems}
                </div>
            </div>

            {/*<button onClick={() => setShowModal(true)} className="button">关于</button>*/}
            {/*    <div className={"modal " + (showModal ? 'is-active' : '')}>*/}
            {/*        <div className="modal-background" onClick={() => setShowModal(false)}></div>*/}
            {/*        <div className="modal-content">*/}
            {/*            <div className="box">*/}
            {/*                <h1>关于</h1>*/}
            {/*                <p>cos收集站</p>*/}
            {/*            </div>*/}
            {/*        </div>*/}
            {/*        <button onClick={() => setShowModal(false)} className="modal-close is-large"*/}
            {/*                aria-label="close"></button>*/}
            {/*    </div>*/}
        </div>

        <footer className="footer">
            <div className="content has-text-centered">
                <p>
                    <strong>sukeb</strong> by leticis.
                    <br/>
                    The source code is licensed GPL-v3.
                </p>
            </div>
        </footer>
    </>)
}

export default App
