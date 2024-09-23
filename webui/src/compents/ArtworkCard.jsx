import React from 'react';
import 'bulma/css/bulma.css';
import {Calendar, File} from 'lucide-react';
import placeholder from '../assets/placeholder.png';

function ArtworkCard({artwork}) {
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
                        <p className="subtitle is-6">{artwork.workId || ""}</p>
                    </div>
                </div>

                <div className="content">
                    <a href={artwork.link}>{artwork.title}</a>
                    <br/>
                    <br/>
                    <div className="columns is-mobile">
                        <div className="column"><span className="icon-text">
                  <span className="icon">
                    <Calendar/>
                  </span>
                  <span>{(new Date(artwork.timestamp * 1000)).toLocaleDateString()}</span>
                </span></div>
                        <div className="column"><span className="icon-text">
  <span className="icon">
    <File/>
  </span>
  <span>{artwork.fileSize}</span>
</span></div>
                    </div>


                </div>
            </div>
        </div>
    </>);
}

export default ArtworkCard;