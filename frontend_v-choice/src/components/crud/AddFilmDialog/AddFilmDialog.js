import { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	DialogContentText,
	TextField,
} from '@material-ui/core'
import AddIcon from '@material-ui/icons/Add'

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
	mainButton: {
		margin: theme.spacing(1)
	},
}));

export default function FormDialog(props) {
	const classes = useStyles();

	const [open, setOpen] = useState(false);
	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);
	const [film, setFilm] = useState({
		title: '',
		description: '',
		year: '',
		genres: [],
		posterPath: "",
	});
	const [poster, setPoster] = useState(null);

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setError(null);
		setMsg(null);
		setFilm({ title: '', description: '', year: '', genres: [] });
		setPoster(null);
	};

	const handleSubmit = () => {
		const formData = new FormData();
		formData.append("title", film.title);
		formData.append("year", film.year);
		formData.append("description", film.description);
		formData.append("poster", poster);
		film.genres.forEach((genre, index) => {
			for (const key in genre) {
				formData.append(`genres[${index}][${key}]`, genre[key]);
			}
		});

		fetch('https://localhost:5001/api/film', {
			method: 'POST',
			body: formData
		})
			.then(async (response) => {
				if (response.status === 401) {
					setError("Недостаточно прав для выполнения операции");
				}
				if (response.status === 400) {
					setError("Проверьте корректность введенных данных");
				}
				if (response.status === 201) {
					const data = await response.json();
					poster === null || (data && poster !== null && data.posterPath !== null)
						? setMsg("Фильм успешно создан")
						: setMsg("Создан без постера. Попробуйте изменить постер позже");
				}
			});
	};

	const handleChangeTitle = (event) => {
		setFilm({ ...film, title: event.target.value });
	};

	const handleChangeYear = (event) => {
		setFilm({ ...film, year: event.target.value });
	};

	const handleChangeDescription = (event) => {
		setFilm({ ...film, description: event.target.value });
	};

	const handleGenresUpdate = (selectedArray) => {
		setFilm({ ...film, genres: [...selectedArray] });
	};

	const handleChangeImage = (event) => {
		setPoster(event.target.files[0]);
	}

	return (
		<>
			<Button variant="outlined" color="primary" onClick={handleOpenDialog} className={classes.mainButton}>
				<AddIcon />
				Добавить фильм
			</Button>

			<Dialog open={open} onClose={handleCloseDialog} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Добавить новый фильм</DialogTitle>
				<DialogContent>
					<MyAlerter msg={msg} error={error} />
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
							className={classes.inputTitle}
							onChange={handleChangeTitle}
							value={film.title}
						/>
						<TextField
							margin="dense"
							id="year"
							label="Год"
							type="number"
							className={classes.inputYear}
							onChange={handleChangeYear}
							value={film.year}
						/>
					</Box>
					<TextField
						margin="dense"
						id="description"
						label="Описание"
						type="input"
						className={classes.item}
						onChange={handleChangeDescription}
						value={film.description}
						fullWidth
					/>
					<input type="file" className={classes.item} onChange={handleChangeImage} />
					<GenresSelector genres={props.genres} selected={[]} onChange={handleGenresUpdate} />
				</DialogContent>
				<DialogActions>
					<Button onClick={handleCloseDialog} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Добавить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	);
}
