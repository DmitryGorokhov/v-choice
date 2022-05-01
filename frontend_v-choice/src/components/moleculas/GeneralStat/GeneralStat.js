import React from 'react'
import { Typography } from '@material-ui/core'

function GeneralStat(props) {
	return (
		<>
			{
				props.data
					? <>
						<Typography variant='subtitle1'>Общее количество фильмов: {props.data.filmsTotal}</Typography>
						<Typography variant='subtitle1'>Количество фильмов с рейтингом: {props.data.filmsRated}</Typography>
						<Typography variant='subtitle1'>Количество фильмов без рейтинга: {props.data.filmsNotRated}</Typography>
						<Typography variant='subtitle1'>Количество фильмов с комментариями: {props.data.filmsCommented}</Typography>
						<Typography variant='subtitle1'>Количество фильмов без комментариев: {props.data.filmsNotCommented}</Typography>
						<Typography variant='subtitle1'>Наименьший год создания фильма: {props.data.minYear}</Typography>
						<Typography variant='subtitle1'>Наибольший год создания фильма: {props.data.maxYear}</Typography>
						<Typography variant='subtitle1'>Общее количество комментариев: {props.data.commentsTotal}</Typography>
						<Typography variant='subtitle1'>Наибольшее количество комментариев к фильму: {props.data.commentsMax}</Typography>
					</>
					: null
			}
		</>
	)
}

export default GeneralStat