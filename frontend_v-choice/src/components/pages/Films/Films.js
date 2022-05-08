import React, { useEffect, useState } from 'react'
import { useParams } from "react-router-dom"

import FilmList from '../../moleculas/FilmList/FilmList'
import { SortingType } from '../../enums/SortingType'
import { FilteringType } from '../../enums/FilteringType'
import { QueryProps } from '../../enums/QueryProps'

function Films() {
	const [genres, setGenres] = useState([]);
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
	}

	recognizeQuery();

	useEffect(() => {
		fetch('https://localhost:5001/api/genre')
			.then(response => response.json())
			.then(result => setGenres(result));
	}, [])

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
				onGenreCreate={handleCreateGenre}
				onGenreUpdate={handleUpdateGenre}
				onGenreDelete={handleDeleteGenre}
			/>
		</>
	)
}

export default Films
