import React from'react';
import 'bulma/css/bulma.css';
import placeholder from '../assets/placeholder.png';

function ArtworkCard({ artwork }) {
    return (<>
        <div className="card">
            <div className="card-image">
                <figure className="image is-4by3">
                    <img
                        src={artwork.imageLink || placeholder}
                        alt="Artwork image"
                        style={{
                            objectFit: 'cover',
                        }}
                    />
                </figure>
            </div>
            <div className="card-content">
                <div className="media">
                    <div className="media-content">
                        <p className="title is-4">{artwork.artist}</p>
                        {/*<p className="subtitle is-6">@johnsmith</p>*/}
                    </div>
                </div>

                <div className="content">
                    <a href={artwork.link}>{artwork.title}</a>
                    <br/>
                    {/*<time dateTime="2016-1-1">11:09 PM - 1 Jan 2016</time>*/}
                </div>
            </div>
        </div>
    </>);
}

export default ArtworkCard;