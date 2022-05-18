import { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogContentText,
	DialogTitle,
	FormControl,
	InputLabel,
	ListItemText,
	MenuItem,
	OutlinedInput,
	Select,
	TextField,
} from '@material-ui/core'
import CreateIcon from '@material-ui/icons/Create'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'
import GenresSelector from '../../atoms/GenresSelector/GenresSelector'
import VideoURLInput from '../../atoms/VideoURLInput/VideoURLInput'
import PersonMultipleSelector from '../../atoms/PersonMultipleSelector/PersonMultipleSelector'

const useStyles = makeStyles((theme) => createStyles({
	flex: {
		display: 'flex',
		justifyContent: 'space-between'
	},
	inputTitle: {
		width: theme.spacing(40)
	},
	inputYear: {
		width: theme.spacing(10)
	},
	item: {
		margin: theme.spacing(2, 0)
	},
	select: {
		width: '250px',
	},
}));

export default function UpdateFilmDialog(props) {
	const classes = useStyles();
	const [open, setOpen] = useState(false);
	const [poster, setPoster] = useState(null);

	const getSelected = (items, list) => {
		const arr = [];
		items.map(item => {
			const found = list.find(i => i.id === item.id);
			if (found) {
				arr.push(found);
			}
		});
		return arr;
	}

	const getSelectedStudio = (item) => {
		if (item === null) {
			return null;
		}

		const found = props.studios.find(i => i.id === item.id);
		return found ? found : null;
	}

	const [state, setState] = useState({
		film: { ...props.film, studio: getSelectedStudio(props.film.studio) },
		selectedGenres: getSelected(props.film.genres, props.genres),
		selectedDirectors: getSelected(props.film.directors, props.persons),
		selectedCast: getSelected(props.film.cast, props.persons),
		error: null,
		msg: null,
	});
	const [videoToken, setVideoToken] = useState(props.film.videoToken !== null ? props.film.videoToken : '');
	const [isVideoTokenValid, setIsVideoTokenValid] = useState(true);

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setPoster(null);
		setState({ ...state, error: null, msg: null });
		setVideoToken('');
		setIsVideoTokenValid(true);
	};

	const handleSubmit = () => {
		if (isVideoTokenValid) {
			const film = { ...state.film };
			film.genres = [...state.selectedGenres];
			film.directors = [...state.selectedDirectors];
			film.cast = [...state.selectedCast];

			const formData = new FormData();
			formData.append("title", film.title);
			formData.append("year", film.year);
			formData.append("description", film.description);
			formData.append("posterPath", film.posterPath);
			formData.append("poster", poster);
			formData.append("videoToken", videoToken);

			if (film.studio != null) {
				formData.append("studio.id", film.studio.id);
				formData.append("studio.name", film.studio.name);
			}

			film.genres.forEach((genre, index) => {
				for (const key in genre) {
					formData.append(`genres[${index}][${key}]`, genre[key]);
				}
			});

			film.directors.forEach((item, index) => {
				formData.append(`directors[${index}][id]`, item.id);
			});

			film.cast.forEach((item, index) => {
				formData.append(`cast[${index}][id]`, item.id);
			});

			const postURL = `https://localhost:5001/api/film/${props.film.id}`;
			fetch(postURL, {
				method: 'PUT',
				body: formData
			})
				.then(async (response) => {
					if (response.status === 401) {
						setState({ ...state, error: "Недостаточно прав для выполнения операции", msg: null });
					}
					if (response.status === 400) {
						setState({ ...state, error: "Проверьте корректность введенных данных", msg: null });
					}
					if (response.status === 200) {
						const data = await response.json();
						let message = "";
						film.videoToken = videoToken;
						if (poster === null) {
							film.posterPath = state.film.posterPath;
							message = "Фильм успешно изменён";
						}
						else {
							if (data && poster !== null && data.path !== null) {
								film.posterPath = data.path;
								message = "Фильм успешно изменён";
							}
							else {
								message = "Выполнено без изменения постера. Попробуйте изменить постер позже";
							}
						}
						setState({ ...state, film: { ...film }, error: null, msg: message });
						props.onUpdate(film);
					}
				});
		}
		else {
			setState({ ...state, error: "Ссылка на видео не соответствует формату", msg: null });
		}
	};

	const handleChangeTitle = (event) => {
		setState({ ...state, film: { ...state.film, title: event.target.value } });
	};

	const handleChangeYear = (event) => {
		setState({ ...state, film: { ...state.film, year: event.target.value } });
	};

	const handleChangeDescription = (event) => {
		setState({ ...state, film: { ...state.film, description: event.target.value } });
	};

	const handleGenresUpdate = (selected) => {
		setState({ ...state, selectedGenres: [...selected] });
	};

	const handleChangeImage = (event) => {
		setPoster(event.target.files[0]);
	}

	const handleDirectorsChange = (list) => {
		setState({ ...state, selectedDirectors: [...list] });
	}

	const handleCastChange = (list) => {
		setState({ ...state, selectedCast: [...list] });
	}

	const handleStudioChange = (event) => {
		setState({ ...state, film: { ...state.film, studio: event.target.value } });
	}

	return (
		<>
			<Button variant="outlined" color="primary" onClick={handleOpenDialog} size="small">
				<CreateIcon />
				Изменить
			</Button>

			<Dialog open={open} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Изменить информацию о фильме</DialogTitle>
				<DialogContent>
					<MyAlerter msg={state.msg} error={state.error} />
					<DialogContentText>
						Заполните информацию о фильме
					</DialogContentText>
					<Box className={classes.item && classes.flex}>
						<TextField
							autoFocus
							margin="dense"
							id="title"
							label="Название"
							type="input"
							value={state.film.title}
							className={classes.inputTitle}
							onChange={handleChangeTitle}
						/>
						<TextField
							margin="dense"
							id="year"
							label="Год"
							type="number"
							value={state.film.year}
							className={classes.inputYear}
							onChange={handleChangeYear}
						/>
					</Box>
					<TextField
						margin="dense"
						id="description"
						label="Описание"
						type="input"
						value={state.film.description}
						className={classes.item}
						onChange={handleChangeDescription}
						fullWidth
					/>
					<VideoURLInput
						className={classes.item}
						token={videoToken}
						setToken={setVideoToken}
						isValid={isVideoTokenValid}
						setIsValid={setIsVideoTokenValid}
					/>
					<Box className={classes.item && classes.flex}>
						<input type="file" className={classes.item} onChange={handleChangeImage} />
						<FormControl>
							<InputLabel id="studio-select-label">Студия</InputLabel>
							<Select
								labelId="studio-select-label"
								id="studio-select"
								value={state.film.studio}
								onChange={handleStudioChange}
								input={<OutlinedInput label="Студия" />}
								className={classes.select}
							>
								<MenuItem key={0} value={null}>
									<ListItemText primary="Не выбрано" />
								</MenuItem>
								{props.studios.map((studio) => (
									<MenuItem key={studio.Id} value={studio}>
										<ListItemText primary={studio.name} />
									</MenuItem>
								))}
							</Select>
						</FormControl>
					</Box>
					<Box className={classes.item && classes.flex}>
						<PersonMultipleSelector
							className={classes.select}
							list={props.persons}
							selected={state.selectedDirectors}
							label="Режиссёр"
							onChange={handleDirectorsChange}
						/>
						<PersonMultipleSelector
							className={classes.select}
							list={props.persons}
							selected={state.selectedCast}
							label="Актёры"
							onChange={handleCastChange}
						/>
					</Box>
					<GenresSelector genres={props.genres} selected={state.selectedGenres} onChange={handleGenresUpdate} />
				</DialogContent>
				<DialogActions>
					<Button onClick={handleCloseDialog} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Применить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	);
}
