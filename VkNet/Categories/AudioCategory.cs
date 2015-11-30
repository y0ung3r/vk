﻿using System.Security.Policy;

namespace VkNet.Categories
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using JetBrains.Annotations;

	using Enums;
	using Enums.Filters;
	using Model;
	using Model.Attachments;
	using Utils;

	/// <summary>
	/// Методы для работы с аудиозаписями.
	/// </summary>
	public class AudioCategory
	{
		private readonly VkApi _vk;

		/// <summary>
		/// Методы для работы с аудиозаписями.
		/// </summary>
		/// <param name="vk">Api vk.com</param>
		internal AudioCategory(VkApi vk)
		{
			_vk = vk;
		}

		/// <summary>
		/// Возвращает количество аудиозаписей пользователя или группы.
		/// </summary>
		/// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество).
		/// Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком "-" —
		/// например, owner_id=-<c>true</c> соответствует идентификатору сообщества ВКонтакте API (club1)</param>
		/// <returns>
		/// Возвращает число, равное количеству аудиозаписей на странице пользователя или группы.
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio" />.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getCount" />.
		/// </remarks>
		/// <example>
		/// Получим количество аудиозаписей Павла Дурова.
		/// <code>
		/// int count = vk.Audio.GetCount(1);
		/// </code></example>
		/// <example>
		/// Получим количество аудиозаписей в группе с id равным 2.
		/// <code>
		/// int count = vk.Audio.GetCount(-2);
		/// </code></example>
		[Pure]
		[ApiVersion("5.40")]
		public int GetCount(long ownerId)
		{
			var parameters = new VkParameters { { "owner_id", ownerId } };

			return _vk.Call("audio.getCount", parameters);
		}

		/// <summary>
		/// Возвращает текст аудиозаписи по идентификатору текста аудиозаписи (<see cref="Audio.LyricsId"/>).
		/// Параметр <paramref name="lyricsId"/> может быть получен с помощью методов <see cref="Get(long,out VkNet.Model.User,System.Nullable{long},System.Collections.Generic.IEnumerable{long},System.Nullable{int},System.Nullable{int})"/>,
		/// <see cref="GetById(System.Collections.Generic.IEnumerable{string})"/> или <see cref="Search"/>.
		/// </summary>
		/// <param name="lyricsId">Идентификатор текста аудиозаписи, информацию о котором необходимо вернуть.</param>
		/// <returns>В случае успеха возвращает найденный текст адиозаписи. В качестве переводов строк в тексте используется \n.</returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getLyrics"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public Lyrics GetLyrics(long lyricsId)
		{
			var parameters = new VkParameters { { "lyrics_id", lyricsId } };

			return _vk.Call("audio.getLyrics", parameters);
		}

		/// <summary>
		/// Возвращает информацию об аудиозаписях. 
		/// </summary>
		/// <param name="audios">
		/// Список строковых идентификаторов аудиозаписей в формате - {owner_id}_{audio_id}.
		/// Если аудиозапись принадлежит группе, то в качестве первого параметра используется -id группы. 
		/// Примеры возможных значений идентификаторов: "2_67859194", "-683495_39822725", "2_63937759".
		/// </param>
		/// <returns>
		/// В случае успеха возвращает информацию о запрошенных аудиозаписях пользователя (группы).
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getById"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public ReadOnlyCollection<Audio> GetById(IEnumerable<string> audios)
		{
			if (!audios.Any())
			{
				throw new ArgumentNullException("audios");
			}

			var parameters = new VkParameters { { "audios", audios } };
			VkResponseArray response = _vk.Call("audio.getById", parameters);

			return response.ToReadOnlyCollectionOf<Audio>(x => x);
		}

		/// <summary>
		/// Возвращает информацию об аудиозаписях. 
		/// </summary>
		/// <param name="audios">
		/// Список строковых идентификаторов аудиозаписей в формате - {owner_id}_{audio_id}.
		/// Если аудиозапись принадлежит группе, то в качестве первого параметра используется -id группы. 
		/// Примеры возможных значений идентификаторов: "2_67859194", "-683495_39822725", "2_63937759".
		/// </param>
		/// <returns>
		/// В случае успеха возвращает информацию о запрошенных аудиозаписях пользователя (группы).
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getById"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public ReadOnlyCollection<Audio> GetById(params string[] audios)
		{
			return GetById((IEnumerable<string>)audios);
		}

		/// <summary>
		/// Возвращает список аудиозаписей группы.
		/// </summary>
		/// <param name="gid">Идентификатор группы, у которой необходимо получить аудиозаписи.</param>
		/// <param name="albumId">Идентификатор альбома, аудиозаписи которого необходимо вернуть (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
		/// <param name="aids">
		/// Список идентификаторов аудиозаписей группы, по которым необходимо получить информацию.
		/// Если список не указан (null), то ограничение на идентификаторы аудиозаписей на накладываются.
		/// </param>
		/// <param name="count">Требуемое количество аудиозаписей.</param>
		/// <param name="offset">Смещение относительно первой найденной аудиозаписи (для выборки определенного подмножества аудиозаписей).</param>
		/// <returns>
		/// В случае успеха возвращает затребованный список аудиозаписей группы.
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.get"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public ReadOnlyCollection<Audio> GetFromGroup(long gid, long? albumId = null, IEnumerable<ulong> aids = null, uint? count = null, uint? offset = null)
		{
			User user;
			return InternalGet("gid", gid, out user, albumId, aids, false, count, offset);
		}

		/// <summary>
		/// Возвращает список аудиозаписей пользователя и краткую информацию о нем.
		/// </summary>
		/// <param name="uid">Идентификатор пользователя, у которого необходимо получить аудиозаписи.</param>
		/// <param name="user">Базовая информация о владельце аудиозаписей - пользователе с идентификатором <paramref name="uid"/> (идентификатор, имя, фотография).</param>
		/// <param name="albumId">Идентификатор альбома пользователя, аудиозаписи которого необходимо получить (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
		/// <param name="aids">Список идентификаторов аудиозаписей пользователя, по которым необходимо получить информацию.</param>
		/// <param name="count">Требуемое количество аудиозаписей.</param>
		/// <param name="offset">Смещение относительно первой найденной аудиозаписи (для выборки определенного подмножества).</param>
		/// <returns>
		/// В случае успеха возвращает затребованный список аудиозаписей пользователя.
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.get"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public ReadOnlyCollection<Audio> Get(ulong uid, out User user, long? albumId = null, IEnumerable<ulong> aids = null, uint? count = null, uint? offset = null)
		{
			return InternalGet("uid", (long)uid, out user, albumId, aids, true, count, offset);
		}

		/// <summary>
		/// Возвращает список аудиозаписей пользователя.
		/// </summary>
		/// <param name="uid">Идентификатор пользователя, у которого необходимо получить аудиозаписи.</param>
		/// <param name="albumId">Идентификатор альбома пользователя, аудиозаписи которого необходимо получить (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
		/// <param name="aids">Список идентификаторов аудиозаписей пользователя, по которым необходимо получить информацию.</param>
		/// <param name="count">Требуемое количество аудиозаписей.</param>
		/// <param name="offset">Смещение относительно первой найденной аудиозаписи (для выборки определенного подмножества).</param>
		/// <returns>В случае успеха возвращает затребованный список аудиозаписей пользователя.</returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.get"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public ReadOnlyCollection<Audio> Get(ulong uid, long? albumId = null, IEnumerable<ulong> aids = null, uint? count = null, uint? offset = null)
		{
			User user;
			return InternalGet("uid", (long)uid, out user, albumId, aids, false, count, offset);
		}

		/// <summary>
		/// Возвращает список аудиозаписей пользователя или сообщества.
		/// </summary>
		/// <param name="paramId">Идентификатор параметра.</param>
		/// <param name="id">Идентификатор пользователя или сообщества, у которого необходимо получить аудиозаписи.</param>
		/// <param name="user">Данные о пользователе.</param>
		/// <param name="albumId">Идентификатор альбома пользователя, аудиозаписи которого необходимо получить (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
		/// <param name="aids">Список идентификаторов аудиозаписей пользователя, по которым необходимо получить информацию.</param>
		/// <param name="needUser"><c>true</c> — возвращать информацию о пользователях, загрузивших аудиозапись.</param>
		/// <param name="count">Требуемое количество аудиозаписей.</param>
		/// <param name="offset">Смещение относительно первой найденной аудиозаписи (для выборки определенного подмножества).</param>
		/// <returns></returns>
		[Pure]
		[ApiVersion("5.40")]
		private ReadOnlyCollection<Audio> InternalGet(
			string paramId,
			long id,
			out User user,
			long? albumId = null,
			IEnumerable<ulong> aids = null,
			bool? needUser = null,
			uint? count = null,
			uint? offset = null)
		{
			// todo Порефакторить метод
			var parameters = new VkParameters
							 {
								 { paramId, id },
								 { "album_id", albumId },
								 { "aids", aids },
								 { "need_user", needUser },
								 { "offset", offset }
							 };
			if (count <= 6000)
			{
				parameters.Add("count", count);
			}
			VkResponseArray response = _vk.Call("audio.get", parameters);

			IEnumerable<VkResponse> items = response.ToList();

			user = null;
			if (needUser.HasValue && needUser.Value && items.Any())
			{
				user = items.First();
				items = items.Skip(1);
			}

			return items.ToReadOnlyCollectionOf<Audio>(r => r);
		}

		/// <summary>
		/// Возвращает адрес сервера для загрузки аудиозаписей. 
		/// </summary>
		/// <returns>
		/// В случае успеха возвращает адрес сервера для загрузки аудиозаписей.
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getUploadServer"/>.
		/// </remarks>
		[Pure]
		[ApiVersion("5.40")]
		public Uri GetUploadServer()
		{
			var response = _vk.Call("audio.getUploadServer", VkParameters.Empty);

			return response["upload_url"];
		}

		/// <summary>
		/// Возвращает список аудиозаписей в соответствии с заданным критерием поиска.
		/// </summary>
		/// <param name="query">Cтрока поискового запроса</param>
		/// <param name="totalCount">Общее количество аудиозаписей удовлетворяющих запросу</param>
		/// <param name="autoComplete">Если этот параметр равен <c>true</c>, возможные ошибки в поисковом запросе будут исправлены. Например, при поисковом запросе <strong>Иуфдуы</strong> поиск будет осуществляться по строке <strong>Beatles</strong></param>
		/// <param name="sort">Вид сортировки</param>
		/// <param name="findLyrics">Будет ли производиться только по тем аудиозаписям, которые содержат тексты.</param>
		/// <param name="count">Количество возвращаемых аудиозаписей (максимум 200).</param>
		/// <param name="offset">Смещение относительно первой найденной аудиозаписи для выборки определенного подмножества.</param>
		/// <returns>Список объектов класса Audio.</returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.search"/>.
		/// </remarks>
		[Pure]
		public ReadOnlyCollection<Audio> Search(
			string query,
			out int totalCount,
			bool? autoComplete = null,
			AudioSort? sort = null,
			bool? findLyrics = null,
			uint? count = null,
			uint? offset = null)
		{
			if (string.IsNullOrEmpty(query))
				throw new ArgumentException("Query is null or empty.", "query");

			var parameters = new VkParameters
							 {
								 { "q", query },
								 { "auto_complete", autoComplete },
								 { "sort", sort },
								 { "lyrics", findLyrics },
								 { "count", count },
								 { "offset", offset }
							 };

			VkResponseArray response = _vk.Call("audio.search", parameters);

			totalCount = response[0];

			return response.Skip(1).ToReadOnlyCollectionOf<Audio>(r => r);
		}

		/// <summary>
		/// Копирует аудиозапись на страницу пользователя или группы. 
		/// </summary>
		/// <param name="audioId">Идентификатор аудиозаписи</param>
		/// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
		/// <param name="groupId">Идентификатор группы, в которую следует копировать аудиозапись. Если параметр не указан, аудиозапись копируется не в группу, а на страницу текущего пользователя.</param>
		/// <returns>Идентификатор созданной аудиозаписи</returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.add"/>.
		/// </remarks>
		public ulong Add(ulong audioId, long ownerId, long? groupId = null)
		{
			var parameters = new VkParameters { { "aid", audioId }, { "oid", ownerId }, { "gid", groupId } };

			return _vk.Call("audio.add", parameters);
		}

		/// <summary>
		/// Удаляет аудиозапись со страницы пользователя или группы.
		/// </summary>
		/// <param name="audioId">Идентификатор аудиозаписи</param>
		/// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
		/// <returns>При успешном удалении аудиозаписи сервер вернет <c>true</c></returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.delete"/>.
		/// </remarks>
		public bool Delete(ulong audioId, long ownerId)
		{
			var parameters = new VkParameters { { "aid", audioId }, { "oid", ownerId } };

			return _vk.Call("audio.delete", parameters);
		}

		/// <summary>
		/// Редактирует данные аудиозаписи на странице пользователя или группы.
		/// </summary>
		/// <param name="audioId">Идентификатор аудиозаписи</param>
		/// <param name="ownerId">Идентификатор владельца аудиозаписи. Если редактируемая аудиозапись находится на странице группы, в этом параметре должно стоять значение, равное -id группы.</param>
		/// <param name="artist">Название исполнителя аудиозаписи.</param>
		/// <param name="title">Название аудиозаписи.</param>
		/// <param name="text">Текст аудиозаписи, если введен.</param>
		/// <param name="noSearch"><c>true</c> - скрывает аудиозапись из поиска по аудиозаписям, <c>false</c> (по умолчанию) - не скрывает.</param>
		/// <param name="genreId">Идентификатор жанра из списка аудио жанров.</param>
		/// <returns>
		/// Идентификатор текста, введенного пользователем
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Artist parameter can not be <see langword="null"/>.
		/// or
		/// Title parameter can not be <see langword="null"/>.
		/// or
		/// Text parameter can not be <see langword="null"/>.
		/// </exception>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio" />.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.edit" />.
		/// </remarks>
		public ulong Edit(ulong audioId, long ownerId, string artist, string title, string text, bool? noSearch = null, AudioGenre? genreId = AudioGenre.Other)
		{
			if (artist == null)
				throw new ArgumentNullException("artist", "Artist parameter can not be null.");

			if (title == null)
				throw new ArgumentNullException("title", "Title parameter can not be null.");

			if (text == null)
				throw new ArgumentNullException("text", "Text parameter can not be null.");

			var parameters = new VkParameters
			{
				{ "aid", audioId },
				{ "oid", ownerId },
				{ "artist", artist },
				{ "title", title },
				{ "text", text },
				{ "no_search", noSearch },
				{ "genre_id", genreId }
			};

			return _vk.Call("audio.edit", parameters);
		}

		/// <summary>
		/// Восстанавливает удаленную аудиозапись пользователя после удаления.
		/// </summary>
		/// <param name="audioId">Идентификатор удаленной аудиозаписи</param>
		/// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
		/// <returns>Удаленная аудиозапись.</returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.restore"/>.
		/// </remarks>
		public Audio Restore(ulong audioId, long? ownerId = null)
		{
			var parameters = new VkParameters { { "aid", audioId }, { "oid", ownerId } };

			return _vk.Call("audio.restore", parameters);
		}

		/// <summary>
		/// Изменяет порядок аудиозаписи, перенося ее между аудиозаписями, идентификаторы которых переданы параметрами <paramref name="after"/> и <paramref name="before"/>.
		/// </summary>
		/// <param name="audioId">Идентификатор аудиозаписи, порядок которой изменяется</param>
		/// <param name="ownerId">Идентификатор владельца изменяемой аудиозаписи</param>
		/// <param name="after">Идентификатор аудиозаписи, после которой нужно поместить аудиозапись. Если аудиозапись переносится в начало, параметр может быть равен нулю.</param>
		/// <param name="before">Идентификатор аудиозаписи, перед которой нужно поместить аудиозапись. Если аудиозапись переносится в конец, параметр может быть равен нулю.</param>
		/// <returns>При успешном изменении порядка аудиозаписи сервер вернет <c>true</c></returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Audio"/>.
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.reorder"/>.
		/// </remarks>
		public bool Reorder(ulong audioId, long ownerId, long after, long before)
		{
			var parameters = new VkParameters { { "aid", audioId }, { "oid", ownerId }, { "after", after }, { "before", before } };

			return _vk.Call("audio.reorder", parameters);
		}

		/// <summary>
		/// Создает пустой альбом аудиозаписей.
		/// </summary>
		/// <param name="title">Название альбома</param>
		/// <param name="groupId">Идентификатор сообщества (если альбом нужно создать в сообществе)</param>
		/// <returns>Идентификатор созданного альбома</returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.addAlbum"/>.
		/// </remarks>
		public ulong AddAlbum(string title, ulong? groupId = null)
		{
			VkErrors.ThrowIfNullOrEmpty(() => title);

			var parameters = new VkParameters
				{
					{"title", title},
					{"group_id", groupId}
				};

			VkResponse response = _vk.Call("audio.addAlbum", parameters);
			return response["album_id"];
		}

		/// <summary>
		/// Редактирует название альбома аудиозаписей.
		/// </summary>
		/// <param name="title">Новое название для альбома</param>
		/// <param name="albumId">Идентификатор альбома</param>
		/// <param name="groupId">Идентификатор сообщества, которому принадлежит альбом</param>
		/// <returns>После успешного выполнения возвращает <c>true</c>.</returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.editAlbum"/>.
		/// </remarks>
		public bool EditAlbum(string title, ulong albumId, ulong? groupId = null)
		{
			VkErrors.ThrowIfNullOrEmpty(() => title);

			var parameters = new VkParameters
				{
					{"title", title},
					{"group_id", groupId},
					{"album_id", albumId}
				};

			return _vk.Call("audio.editAlbum", parameters);
		}

		/// <summary>
		/// Удаляет альбом аудиозаписей.
		/// </summary>
		/// <param name="albumId">Идентификатор альбома</param>
		/// <param name="groupId">Идентификатор сообщества, которому принадлежит альбом</param>
		/// <returns>После успешного выполнения возвращает <c>true</c>.</returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.deleteAlbum"/>.
		/// </remarks>
		public bool DeleteAlbum(ulong albumId, ulong? groupId = null)
		{
			var parameters = new VkParameters
				{
					{"album_id", albumId},
					{"group_id", groupId}

				};

			return _vk.Call("audio.deleteAlbum", parameters);
		}

		/// <summary>
		/// Возвращает список аудиозаписей из раздела "Популярное".
		/// </summary>
		/// <param name="onlyEng"><c>true</c> – возвращать только зарубежные аудиозаписи. <c>false</c> – возвращать все аудиозаписи. (по умолчанию) </param>
		/// <param name="genre">Идентификатор жанра </param>
		/// <param name="count">Количество возвращаемых аудиозаписей</param>
		/// <param name="offset">Смещение, необходимое для выборки определенного подмножества аудиозаписей</param>
		/// <returns>Список аудиозаписей из раздела "Популярное"</returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getPopular"/>.
		/// </remarks>
		[Pure]
		public ReadOnlyCollection<Audio> GetPopular(bool onlyEng = false, AudioGenre? genre = null, uint? count = null, uint? offset = null)
		{
			var parameters = new VkParameters
				{
					{"only_eng", onlyEng},
					{"genre_id", genre},
					{"offset", offset}
				};
			if (count <= 1000)
			{
				parameters.Add("count", count);
			}
			VkResponseArray response = _vk.Call("audio.getPopular", parameters);

			return response.ToReadOnlyCollectionOf<Audio>(x => x);
		}

		/// <summary>
		/// Возвращает список альбомов аудиозаписей пользователя или группы.
		/// </summary>
		/// <param name="ownerid">Идентификатор пользователя или сообщества, у которого необходимо получить список альбомов с аудио.</param>
		/// <param name="count">Количество альбомов, которое необходимо вернуть.</param>
		/// <param name="offset">Смещение, необходимое для выборки определенного подмножества альбомов.</param>
		/// <returns>
		/// После успешного выполнения возвращает массив альбомов аудиоальбомов <see cref="AudioAlbum"/>.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getAlbums"/>.
		/// </remarks>
		[Pure]
		public ReadOnlyCollection<AudioAlbum> GetAlbums(long ownerid, uint? count = null, uint? offset = null)
		{
			var parameters = new VkParameters
				{
					{"owner_id", ownerid},
					{"count", count},
					{"offset", offset}
				};

			VkResponseArray response = _vk.Call("audio.getAlbums", parameters);

			return response.Skip(1).ToReadOnlyCollectionOf<AudioAlbum>(x => x);
		}

		/// <summary>
		/// Перемещает аудиозаписи в альбом.
		/// </summary>
		/// <param name="albumId">Идентификатор альбома, в который нужно переместить аудиозаписи</param>
		/// <param name="audioIds">Идентификаторы аудиозаписей, которые требуется переместить</param>
		/// <param name="groupId">Идентификатор сообщества, в котором размещены аудиозаписи. Если параметр не указан, работа ведется с аудиозаписями текущего пользователя</param>
		/// <returns>После успешного выполнения возвращает <c>true</c></returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.moveToAlbum"/>.
		/// </remarks>
		public bool MoveToAlbum(ulong albumId, IEnumerable<ulong> audioIds, ulong? groupId = null)
		{
			var parameters = new VkParameters
			{
				{"album_id", albumId},
				{"group_id", groupId},
				{"audio_ids", audioIds}
			};

			return _vk.Call("audio.moveToAlbum", parameters);
		}

		/// <summary>
		/// Возвращает список рекомендуемых аудиозаписей на основе списка воспроизведения заданного пользователя или на основе одной выбранной аудиозаписи.
		/// </summary>
		/// <param name="userId">Идентификатор пользователя для получения списка рекомендаций на основе его набора аудиозаписей (по умолчанию — идентификатор 
		/// текущего пользователя).</param>
		/// <param name="count">Количество возвращаемых аудиозаписей.</param>
		/// <param name="offset">Смещение относительно первой найденной аудиозаписи для выборки определенного подмножества.</param>
		/// <param name="shuffle"><c>true</c> — включен случайный порядок.</param>
		/// <param name="targetAudio">Идентификатор аудиозаписи, на основе которой будет строиться список рекомендаций. Используется вместо параметра uid. 
		/// Идентификатор представляет из себя разделённые знаком подчеркивания id пользователя, которому принадлежит аудиозапись, и id самой аудиозаписи. 
		/// Если аудиозапись принадлежит сообществу, то в качестве первого параметра используется -id сообщества.</param>
		/// <returns>Список рекомендуемых аудиозаписей.</returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getRecommendations"/>.
		/// </remarks>
		[Pure]
		public ReadOnlyCollection<Audio> GetRecommendations(ulong? userId = null, uint? count = null, uint? offset = null, bool shuffle = true, string targetAudio = "")
		{
			var parameters = new VkParameters
				{
					{"target_audio", targetAudio},
					{"user_id", userId},
					{"offset", offset},
					{"count", count},
					{"shuffle", shuffle}
				};

			VkResponseArray response = _vk.Call("audio.getRecommendations", parameters);

			return response.ToReadOnlyCollectionOf<Audio>(x => x);
		}

		/// <summary>
		/// Транслирует аудиозапись в статус пользователю или сообществу.
		/// </summary>
		/// <param name="audio">Идентификатор аудиозаписи, которая будет отображаться в статусе, в формате owner_id_audio_id. Например, 1_190442705. 
		/// Если параметр не указан, аудиостатус указанных сообществ и пользователя будет удален.</param>
		/// <param name="targetIds">Перечисленные через запятую идентификаторы сообществ и пользователя, которым будет транслироваться аудиозапись. 
		/// Идентификаторы сообществ должны быть заданы в формате "-gid", где gid - идентификатор сообщества. Например, 1,-34384434. По умолчанию аудиозапись 
		/// транслируется текущему пользователю.</param>
		/// <returns>В случае успешного выполнения возвращает массив идентификаторов сообществ и пользователя, которым был установлен или удален аудиостатус.</returns>
		public ReadOnlyCollection<long> SetBroadcast(string audio, IEnumerable<long> targetIds)
		{
			VkErrors.ThrowIfNullOrEmpty(() => audio);

			var parameters = new VkParameters
			{
				{"audio", audio},
				{"target_ids", targetIds}
			};

			VkResponseArray response = _vk.Call("audio.setBroadcast", parameters);

			return response.ToReadOnlyCollectionOf<long>(x => x);
		}

		/// <summary>
		/// Сохраняет аудиозаписи после успешной загрузки.
		/// </summary>
		/// <param name="server">Параметр, возвращаемый в результате загрузки аудиофайла на сервер. </param>
		/// <param name="audio">Параметр, возвращаемый в результате загрузки аудиофайла на сервер.</param>
		/// <param name="hash">Параметр, возвращаемый в результате загрузки аудиофайла на сервер.</param>
		/// <param name="artist">Автор композиции. По умолчанию берется из ID3 тегов.</param>
		/// <param name="title">Название композиции. По умолчанию берется из ID3 тегов. </param>
		/// <returns>Возвращает массив из объектов с загруженными аудиозаписями.</returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.save"/>.
		/// </remarks>
		[ApiVersion("5.21")]
		public ReadOnlyCollection<Audio> Save(long server, string audio, string hash = null, string artist = null, string title = null)
		{
			VkErrors.ThrowIfNullOrEmpty(() => audio);
			var parameters = new VkParameters
			{
				{"server", server},
				{"audio", audio},
				{"hash", hash},
				{"artist", artist},
				{"title", title}
			};

			VkResponseArray response = _vk.Call("audio.save", parameters);
			return response.ToReadOnlyCollectionOf<Audio>(x => x);
		}


		/// <summary>
		/// Возвращает список друзей, которые транслируют музыку в статус.
		/// </summary>
		/// <param name="active"><c>true</c> — будут возвращены только друзья и сообщества, которые транслируют музыку в данный момент. По умолчанию возвращаются все.</param>
		/// <returns>
		/// После успешного выполнения возвращает список объектов друзей с дополнительным полем status_audio — объект аудиозаписи, 
		/// установленной в статус (если аудиозапись транслируется в текущей момент).
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getBroadcastList"/>.
		/// </remarks>
		public ReadOnlyCollection<User> GetBroadcastListFriends(bool active = false)
		{
			var parameters = new VkParameters
			{
				{"filter", "friends"},
				{"active", active}
			};

			VkResponseArray response = _vk.Call("audio.getBroadcastList", parameters);
			return response.ToReadOnlyCollectionOf<User>(x => x);
		}

		/// <summary>
		/// Возвращает список сообществ пользователя, которые транслируют музыку в статус.
		/// </summary>
		/// <param name="active"><c>true</c> — будут возвращены только друзья и сообщества, которые транслируют музыку в данный момент. По умолчанию возвращаются все.</param>
		/// <returns>
		/// После успешного выполнения возвращает список объектов сообществ с дополнительным полем status_audio — объект аудиозаписи, 
		/// установленной в статус (если аудиозапись транслируется в текущей момент).
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте <see href="http://vk.com/dev/audio.getBroadcastList"/>.
		/// </remarks>
		public ReadOnlyCollection<Group> GetBroadcastListGroup(bool active = false)
		{
			var parameters = new VkParameters
			{
				{"filter", "groups"},
				{"active", active}
			};

			VkResponseArray response = _vk.Call("audio.getBroadcastList", parameters);
			return response.ToReadOnlyCollectionOf<Group>(x => x);
		}
	}
}