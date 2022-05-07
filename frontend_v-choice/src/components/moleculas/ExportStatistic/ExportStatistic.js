import { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Button,
	FormControl,
	InputLabel,
	MenuItem,
	Select,
	Typography
} from '@material-ui/core'
import { Link } from 'react-router-dom'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'

const useStyles = makeStyles((theme) => createStyles({
	item: {
		margin: theme.spacing(2, 0),
	}
}));

export default function ExportStatistic(props) {
	const classes = useStyles();

	const showLink = (link) => {
		return (
			<Typography>
				Файл подготовлен. Вы можете получить его по <Link to={`/${link}`} target="_blank">ссылке</Link>.
			</Typography >
		)
	}

	const showChanged = (link) => {
		return (
			<Typography>
				Столбцы для сортировок изменены. Вы можете выгрузить новый отчет <br />
				Прошлый отчет всё ещё доступен по <Link to={`/${link}`} target="_blank">ссылке</Link>
			</Typography >
		)
	}

	const [msg, setMsg] = useState(props.link && props.link !== undefined ? () => showLink(props.link) : null);
	const [error, setError] = useState(null);
	const [changed, setChanged] = useState(false);

	const [filmSortingType, setFilmSortingType] = useState(0);
	const [genreSortingType, setGenreSortingType] = useState(0);

	const handleSubmit = () => {
		fetch('https://localhost:5001/api/statistic', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify({ filmSortingType: filmSortingType, genreSortingType: genreSortingType })
		})
			.then(response => response.json())
			.then(data => {
				if (data && data.link !== undefined) {
					setMsg(showLink(data.link));
					setChanged(false);
					props.saveLink(data.link);
				}
				else {
					setError("Произошла ошибка. Попробуйте снова позже.");
				}
			})
			.catch(_ => setError("Произошла ошибка. Попробуйте снова позже."));
	};

	const handleChangeFilmSortingType = (event) => {
		setFilmSortingType(event.target.value);
		if (!changed) {
			setChanged(true);

			if (props.link && props.link !== undefined) {
				setMsg(() => showChanged(props.link));
			}
		}
	};

	const handleChangeGenreSortingType = (event) => {
		setGenreSortingType(event.target.value);
		if (!changed) {
			setChanged(true);

			if (props.link && props.link !== undefined) {
				setMsg(() => showChanged(props.link));
			}
		}
	};

	return (
		<>
			<MyAlerter msg={msg} error={error} />
			<Typography variant='subtitle1' className={classes.item}>
				Укажите столбцы сортировки таблиц статистики фильмов и жанров
			</Typography>
			<FormControl fullWidth className={classes.item}>
				<InputLabel id="film-sorting-type-select-label">Отсортировать статистику фильмов по столбцу</InputLabel>
				<Select
					labelId="film-sorting-type-select-label"
					id="film-sorting-type-select"
					value={filmSortingType}
					onChange={handleChangeFilmSortingType}
				>
					<MenuItem value={0}>Количество просмотров</MenuItem>
					<MenuItem value={1}>Рейтинг</MenuItem>
					<MenuItem value={2}>Количество оценок</MenuItem>
					<MenuItem value={3}>Количество комментариев</MenuItem>
					<MenuItem value={4}>Количество в Избранном</MenuItem>
				</Select>
			</FormControl>

			<FormControl fullWidth className={classes.item}>
				<InputLabel id="genre-sorting-type-select-label">Отсортировать статистику жанров по столбцу</InputLabel>
				<Select
					labelId="genre-sorting-type-select-label"
					id="genre-sorting-type-select"
					value={genreSortingType}
					onChange={handleChangeGenreSortingType}
				>
					<MenuItem value={0}>Количество фильмов</MenuItem>
					<MenuItem value={1}>Количество запросов для фильтрации</MenuItem>
				</Select>
			</FormControl>
			<Button onClick={handleSubmit} color="primary" disabled={msg !== null && !changed}>
				Выгрузить
			</Button>
		</>
	);
}
