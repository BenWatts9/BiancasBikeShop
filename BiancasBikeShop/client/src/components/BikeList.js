import { useState, useEffect } from "react"
import BikeCard from "./BikeCard"
import { getBikes } from "../bikeManager"


export default function BikeList({setDetailsBikeId}) {
    const [bikes, setBikes] = useState([])

    const getAllBikes = () => {
        //implement functionality here...
        getBikes().then(bikes=>setBikes(bikes))
    }

    useEffect(() => {
        getAllBikes()
    }, [])
    return (
        <>
        <h2>Bikes</h2>
        {/* Use BikeCard component here to list bikes...*/}
        {bikes.map((bike)=>(
            <BikeCard bike={bike} key={bike.id} SetDetailsBikeId={bike.id} />
        ))}
        </>
    )
}