import { useState, useEffect } from "react"
import BikeCard from "./BikeCard"
import { getBikes } from "../bikeManager"


export default function BikeList({setDetailsBikeId}) {
    const [bikes, setBikes] = useState([])

    const getAllBikes = () => {
        getBikes().then(bikes=>setBikes(bikes))
    }

    useEffect(() => {
        getAllBikes()
    }, [])
    return (
        <>
        <h2>Bikes</h2>
        {bikes.map((bike)=>(
            <BikeCard bike={bike} key={bike.id} setDetailsBikeId={setDetailsBikeId} />
        ))}
        </>
    )
}