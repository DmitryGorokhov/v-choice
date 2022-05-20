import React, { useEffect, useState } from 'react'
import { useParams } from "react-router-dom"

import FilmList from '../../moleculas/FilmList/FilmList'
import { SortingType } from '../../enums/SortingType'
import { FilteringType } from '../../enums/FilteringType'
import { QueryProps } from '../../enums/QueryProps'

function Films() {
	const [genres, setGenres] = useState([]);
	const [studios, setStudios] = useState([]);
	const [persons, setPersons] = useState([]);
	const [filterData, setFilterData] = useState({
		yearMin: 0,
		yearMax: new Date().getFullYear(),
		rateMin: 0,
		rateMax: 10,
	});
	const { slug } = useParams();

	const recognizeQuery = () => {
		if (slug) {
			slug.split('&').forEach(line => {
				const pair = line.split('=');
				const failedKeys = [];
				const failedValues = [];

				switch (pair[0]) {
					case QueryProps.Page:
					case QueryProps.Count:
					case QueryProps.GenreId:
					case QueryProps.ActorId:
					case QueryProps.DirectorId:
					case QueryProps.StudioId:
					case QueryProps.YearMin:
					case QueryProps.YearMax:
					case QueryProps.RateMin:
					case QueryProps.RateMax:
						!isNaN(Number(pair[1]))
							? params[pair[0]] = Number(pair[1])
							: failedValues.push(pair[1]);
						break;

					case QueryProps.SortBy:
						SortingType[pair[1]] !== undefined
							? params[pair[0]] = SortingType[pair[1]]
							: failedValues.push(pair[1]);
						break;

					case QueryProps.Filter:
						switch (pair[1]) {
							case FilteringType.NotSet:
								params.withCommentsOnly = false;
								params.withRateOnly = false;
								break;
							case FilteringType.Rated:
								params.withCommentsOnly = false;
								params.withRateOnly = true;
								break;
							case FilteringType.Commented:
								params.withCommentsOnly = true;
								params.withRateOnly = false;
								break;
							case FilteringType.RatedCommented:
								params.withCommentsOnly = true;
								params.withRateOnly = true;
								break;
							default:
								failedValues.push(pair[1]);
								break;
						}
						break;

					case QueryProps.Search:
						params[pair[0]] = pair[1];
						break;

					default:
						failedKeys.push(pair[0]);
						break;
				}
			});
		}
	}

	const params = {
		page: 1,
		count: 3,
		"genre-id": 0,
		"sort-by": SortingType['not-set'],
		withCommentsOnly: false,
		withRateOnly: false,
		"y-min": 0,
		"y-max": new Date().getFullYear(),
		"r-min": 0,
		"r-max": 10,
		dir: 0,
		act: 0,
		st: 0,
		search: "",
	}

	recognizeQuery();

	useEffect(() => {
		fetch('https://localhost:5001/api/genre')
			.then(response => response.json())
			.then(result => setGenres(result))
			.catch(_ => _);
		fetch('https://localhost:5001/api/studio')
			.then(response => response.json())
			.then(result => setStudios(result))
			.catch(_ => _);
		fetch('https://localhost:5001/api/person/all')
			.then(response => response.json())
			.then(result => setPersons(result))
			.catch(_ => _);
		fetch('https://localhost:5001/api/statistic/catalog-filters')
			.then(response => response.json())
			.then(result => setFilterData(result))
			.catch(_ => _);
	}, [])

	const handleCreateStudio = (studio) => {
		setStudios([...studios, studio]);
	}

	const handleUpdateStudio = (studio) => {
		let arr = [...studios];
		let found = arr.find(s => s.id === studio.id);
		if (found) {
			found.value = studio.value;
		}
		setStudios([...arr]);
	}

	const handleDeleteStudio = (studio) => {
		setStudios(studios.filter(s => s.id !== studio.id));
	}

	const handleCreateGenre = (genre) => {
		setGenres([...genres, genre]);
	}

	const handleUpdateGenre = (genre) => {
		let arr = [...genres];
		let found = arr.find(g => g.id === genre.id);
		if (found) {
			found.value = genre.value;
		}
		setGenres([...arr]);
	}

	const handleDeleteGenre = (genre) => {
		setGenres(genres.filter(g => g.id !== genre.id));
	}

	return (
		<>
			<FilmList
				{...params}
				genres={genres}
				studios={studios}
				persons={persons}
				genreMethods={{ onCreate: handleCreateGenre, onUpdate: handleUpdateGenre, onDelete: handleDeleteGenre }}
				studioMethods={{ onCreate: handleCreateStudio, onUpdate: handleUpdateStudio, onDelete: handleDeleteStudio }}
				filterData={filterData}
			/>
		</>
	)
}

export default Films
