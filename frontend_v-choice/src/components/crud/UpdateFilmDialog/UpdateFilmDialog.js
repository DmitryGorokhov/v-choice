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
	TextField,
} from '@material-ui/core'
import CreateIcon from '@material-ui/icons/Create'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'
import GenresSelector from '../../atoms/GenresSelector/GenresSelector'

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
}));

export default function UpdateFilmDialog(props) {
	const classes = useStyles();
	const [open, setOpen] = useState(false);
	const [poster, setPoster] = useState(null);

	const getSelectedGenres = () => {
		const arr = [];
		props.film.genres.map(g => {
			const found = props.genres.find(i => i.id === g.id);
			if (found) {
				arr.push(found);
			}
		});
		return arr;
	}

	const [state, setState] = useState({
		film: { ...props.film },
		selectedGenres: getSelectedGenres(),
		error: null,
		msg: null,
	});

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setPoster(null);
		setState({ ...state, error: null, msg: null })
	};

	const handleSubmit = () => {
		const film = { ...state.film };
		film.genres = [...state.selectedGenres];

		const formData = new FormData();
		formData.append("title", film.title);
		formData.append("year", film.year);
		formData.append("description", film.description);
		formData.append("posterPath", film.posterPath);
		formData.append("poster", poster);
		film.genres.forEach((genre, index) => {
			for (const key in genre) {
				formData.append(`genres[${index}][${key}]`, genre[key]);
			}
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
					if (poster === null || (data && poster !== null && data.path !== null)) {
						film.posterPath = data.path;
						message = "Фильм успешно изменён";
					}
					else {
						message = "Выполнено без изменения постера. Попробуйте изменить постер позже";
					}

					setState({ ...state, film: { ...film }, error: null, msg: message });
				}
				props.onUpdate(film);
			});
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

	return (
		<>
			<Button variant="outlined" color="primary" onClick={handleOpenDialog}>
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
					<input type="file" className={classes.item} onChange={handleChangeImage} />
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
